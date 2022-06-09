import sys
import os
import datetime
import numpy as np
from astropy.io import fits
from ProgressPrint import Progress
from math_actions.Addition import Addition
from math_actions.Median import Median
from math_actions.Division import Division
from math_actions.Minus import Minus
from math_actions.Multiplication import Multiplication
from datetime import datetime

progress = Progress(module_name="FitsMath", stages=1)


#
# def show_exception_and_exit(exc_type, exc_value, tb):
#     import traceback
#     error = ""
#     for e in traceback.format_exception(exc_type, exc_value, tb):
#         error += e
#         error += '\n'
#     progress.eprint(error)
#     sys.exit(-1)
#
#
# sys.excepthook = show_exception_and_exit


def eprint(*args, **kwargs):
    print(*args, file=sys.stderr, **kwargs)


def get_filenames_from_folder(folder_path):
    if os.path.exists(folder_path) and os.path.isdir(folder_path):
        only_files = [
            file for file in os.listdir(folder_path)
            if os.path.isfile(os.path.join(folder_path, file))
        ]
        for file in only_files:
            if is_fit_file(file) is False:
                progress.eprint('File in folder is not in fit format')
                sys.exit(1)
        only_files = [(folder_path + '\\' + file) for file in only_files]
        return only_files
    progress.eprint('Folder is not exist or its not a directory')
    sys.exit(1)


def is_fit_file(path: str) -> bool:
    """
    check if file is in fit format
    """
    if path.endswith('.fit') or path.endswith('.fits') is True:
        return True
    else:
        return False


def convert_path_to_files(paths):
    input_files: list = []
    for path in paths:
        if is_fit_file(path):
            input_files.append(path)
        else:
            input_files += get_filenames_from_folder(path)

    files_amount = len(input_files)
    if files_amount == 0:
        progress.eprint("no input file detected")
        sys.exit(1)
    return input_files


def fill_header(image_type: str, **kwargs: dict):
    """
    creates header by user args
    :return: the header
    """
    header = fits.Header()

    # Add history
    date = datetime.now().strftime("%d/%m/%Y %H:%M")
    header['history'] = f'= created on {date}'

    # Write type of file
    header['IMAGETYP'] = image_type

    # Add dictionary
    for key, value in kwargs.items():
        header[key] = value

    return header


def save_fit(save_path: str, image_data, base_header, copy_header: str = None, overwrite=True):
    # Copy the header
    if copy_header is not None:
        hdr = fits.getheader(copy_header)
        for key, value in hdr.items():
            base_header[key] = value

    # Save fits file
    hdu = fits.PrimaryHDU(data=image_data, header=base_header)
    hdul = fits.HDUList([hdu])
    hdul.writeto(save_path, overwrite=overwrite)  # check for errors


def time_from_path(path):
    header = fits.getheader(path)
    exposure_time = header['EXPOSURE']
    return exposure_time


def median_time(paths_list: list):
    if paths_list is None:
        return None
    np_arr = np.array(paths_list)
    return np.mean(np_arr)


def calibration_compute_process(paths, output_master_bias, output_master_dark, output_master_flat,
                                output_calibration_file, output_calibration_folder):
    """
    Function to compute calibration and output the wanted photos
    :param paths:
    :param output_master_bias:
    :param output_master_dark:
    :param output_master_flat:
    :param output_calibration_file:
    :param output_calibration_folder:
    :return:
    """
    global progress

    # convert path from argument to files
    for path in paths:
        paths[path] = convert_path_to_files(paths[path])

    # masterBias
    outcome_array = None
    if 'bias' in paths:
        outcome_array = []
        for bias_path in paths:
            bias = fits.getdata(bias_path)
            outcome_array.append(bias)
    master_bias = Median(outcome_array).compute()
    if output_master_bias != '' and master_bias is not None:
        save_fit(output_master_bias, master_bias, fill_header('masterBias'))

    outcome_array = None
    # masterDark
    if 'dark' in paths:
        outcome_array = []
        for dark_path in paths:
            dark = fits.getdata(dark_path)
            outcome_array.append(dark)
    array_of_images = Minus(outcome_array, master_bias).compute()
    master_dark = Median(array_of_images).compute()
    if output_master_dark != '' and master_dark is not None:
        save_fit(output_master_dark, master_dark, fill_header('masterDark'))

    dark_time_in_header_average = median_time(outcome_array)

    outcome_array = None
    # masterFlat
    if 'flat' in paths:
        outcome_array = []
        for flat_path in paths['flat']:
            # Normalization
            flat = fits.getdata(flat_path)
            flat_time_in_header = time_from_path(flat_path)
            exposure_with_dark = Multiplication(master_dark,
                                                flat_time_in_header if flat_time_in_header is not None else None).compute()
            mana = Division(exposure_with_dark, dark_time_in_header_average).compute()
            middle_score = Addition(mana, master_bias).compute()
            outcome = Minus(flat, middle_score).compute()
            outcome_array.append(outcome)
    master_flat = Median(outcome_array).compute()
    if output_master_flat != '' and master_flat is not None:
        save_fit(output_master_flat, master_flat, fill_header('masterFlat'))

    # Calibration
    if 'light' in paths:
        for i, light_path in enumerate(paths['light']):
            # Normalization
            light_time_in_header = time_from_path(light_path)
            first_phase = Multiplication(master_dark, light_time_in_header).compute()
            normalization_per_light = Division(first_phase, dark_time_in_header_average).compute()
            light = fits.getdata(light_path)
            matrix_a = Addition(master_bias, normalization_per_light).compute()
            matrix_b = Minus(light, matrix_a).subtract_two_images()
            calibration_output = Division(matrix_b, master_flat).compute()

            # outcome image and save
            output_file_name = output_calibration_folder + '/' + str(i) + '.fit'
            save_fit(output_file_name, calibration_output, fill_header('calibration - part ' + str(i)))
