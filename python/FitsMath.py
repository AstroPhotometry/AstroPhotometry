import sys
import os
import datetime
import numpy as np
from astropy.io import fits
from ProgressPrint import Progress
from math_actions.Addition import Addition
from math_actions.Average import Average
from math_actions.Division import Division
from math_actions.Minus import Minus

progress = Progress(module_name="FitsMath", stages=1)


def show_exception_and_exit(exc_type, exc_value, tb):
    import traceback
    error = ""
    for e in traceback.format_exception(exc_type, exc_value, tb):
        error += e
        error += '\n'
    progress.eprint(error)
    sys.exit(-1)


sys.excepthook = show_exception_and_exit


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


def fill_header():
    """
    TODO: fill the function
    :return:
    """
    pass


def open_fits_files(files):
    """
    Open fits files
    :return: array of matrix that represent the images inside the fit files
    """
    arr_of_images = []
    for file in files:
        with fits.open(file, mode='readonly') as f_d_image:
            out_picture = f_d_image[0].data[:, :]
        arr_of_images.append(out_picture)
    return arr_of_images


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

    # open all files as array of matrix
    arr_of_matrix = open_fits_files(paths)

    # Examples of Math Actions:
    image_arr = Addition(arr_of_matrix).compute()
    image_arr = Average(arr_of_matrix).compute()
    array_of_images = Minus(arr_of_matrix[1:], arr_of_matrix[0]).compute()
    array_of_images = Division(arr_of_matrix[1:], arr_of_matrix[0]).compute()

    # progress = Progress(
    #     'calibration', stages=files_amount +
    #                         3)  # Pass on all images + create save and done of fit file
    # progress.cprint("started working")
    #
    #
    # arr_of_images = []
    # with fits.open(input_files[0], mode='readonly') as base_file:
    #     out_picture = base_file[0].data[:, :]
    #     arr_of_images.append(out_picture)
    #
    # progress.cprint("read file: " + input_files[0])
    #
    # for input_file in input_files[1:]:
    #     with fits.open(input_file, mode='readonly') as next_file:
    #         progress.cprint("read file: " + input_file)
    #
    #         # Average
    #         if args.A is True:
    #             arr_of_images.append(next_file[0].data[:, :])
    #         # Minus
    #         elif args.m is True:
    #             tmp = out_picture - next_file[
    #                                     0].data[:, :]  # Has tmp for mixing ints and floats array math
    #             out_picture = tmp
    #         # Multiplication
    #         elif args.M is True:
    #             out_picture *= next_file[0].data[:, :]
    #         # Addition
    #         elif args.a is True:
    #             arr_of_images.append(next_file[0].data[:, :])
    #         # Division
    #         elif args.d is True:
    #             if next_file[0].data[:, :] == 0:
    #                 progress.eprint('Division by 0')
    #                 sys.exit(1)
    #             out_picture = base_file[0].data[:, :]
    #             out_picture = out_picture / next_file[0].data[:, :]
    #         else:
    #             progress.eprint(f"no module name detected in: {args}")
    #             sys.exit(1)
    #
    # if args.A is True:
    #     out_picture = np.mean(arr_of_images, axis=0)
    # elif args.a is True:
    #     out_picture = np.sum(arr_of_images, axis=0)
    # np.subtract

    # progress.cprint("creating fit file")
    # hdr = fits.Header()
    # date = datetime.datetime.now().strftime("%d/%m/%Y %H:%M")
    # hdr['history'] = f"= edited on {date}"
    # hdu = fits.PrimaryHDU(data=out_picture, header=hdr)
    # hdul = fits.HDUList([hdu])
    # progress.cprint("saving fit file")
    # hdul.writeto(output_file_path,
    #              overwrite=overwrite_flag)  # check for errors
    #
    # progress.cprint("done, saved in " + output_file_path)
