import argparse

from FitsToPNG import main_run
from FitsMath import calibration_compute_process
from JsonConvert import JsonConvert


def argument_handling():
    """
    Method to deal with arguments parsing
    :return: file path to fits file and path to a new png file
    """
    parser = argparse.ArgumentParser()
    parser.add_argument('-j', '--json',
                        required=True,
                        nargs='+',
                        type=str,
                        help='Insert json object by the pre-definition')
    args = parser.parse_args()
    return args.json


def fits_to_png_proc(path_arr: list):
    fits_path, png_path = path_arr
    main_run(fits_path, png_path)


def validate_files(bias_file, dark_file, flats_file, light_file) -> dict:
    """
    :param bias_file:
    :param dark_file:
    :param flats_file:
    :param light_file:
    :return: dict with the existing files array as values and keys as names
    """
    dict_files = {}
    if bias_file:
        dict_files['bias'] = bias_file
    if dark_file:
        dict_files['dark'] = bias_file
    if flats_file:
        dict_files['flat'] = bias_file
    if light_file:
        dict_files['light'] = bias_file
    return dict_files


if __name__ == '__main__':
    #     sys.argv = ['main.py', '-j',
    #                 '{"fitsToPNG":"","bias": ["file1", "file2", "file3"],"dark": ["file1", "file2", "file3"],"flats": ["file1", "file2", "file3"],"light": ["file1", "file2", "file3"],"outputMasterBias": "path","outputMasterDark": "path","outputMasterFlat": "path","outputCallibrationFile": "path","outputCallibratedFolder": "path"}']
    json_data = argument_handling()
    data = JsonConvert(json_data)
    fits_to_png, bias, dark, flats, light, output_master_bias, output_master_dark, output_master_flat, output_calibration_file, output_calibration_folder = data.run()
    if fits_to_png != '':
        fits_to_png_proc(fits_to_png)
    else:
        files = validate_files(bias, dark, flats, light)
        calibration_compute_process(files, output_master_bias, output_master_dark, output_master_flat,
                                    output_calibration_file, output_calibration_folder)
