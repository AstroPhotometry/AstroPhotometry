from array import array
import numpy as np
from astropy.io import fits
import sys


def eprint(*args, **kwargs):
    print(*args, file=sys.stderr, **kwargs)


def str_tuple_to_arr(tup: str) -> list[int]:
    return [
        int(s) for s in tup.strip("(").strip(")").split(",") if s.isdigit()
    ]


# TODO: return or print the output file name for piping
def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    eprint(
        f"""usage: {split_filename[-1]} -p <file name> -r <resolution of histogram> -sxy (sx,sy) -exy (ex,ey) -s
        -p for piping filename, default = false
        -r resolution of histogram, default = 100
        -sxy start position in x,y coordinates, default = 0
        -exy end position in x,y coordinates, default = end
        -s show plt on screen, default = false
        """)


def parser(argv: list[str]):
    if "-r" in argv:
        pos = argv.index("-r")
        param: str = argv[pos + 1]
        if param.isdigit():
            argv.remove("-r")
            argv.remove(param)
            resolution: int = int(param)
        else:
            print_usage(argv[0])
            exit(1)
    else:
        resolution: int = 100  # default

    if "-sxy" in argv:
        pos = argv.index("-sxy")
        param: str = argv[pos + 1]
        num_arr: list[int] = str_tuple_to_arr(param)
        if len(num_arr) == 2:
            argv.remove("-sxy")
            argv.remove(param)
            sx: int = num_arr[0]
            sy: int = num_arr[1]
        else:
            print_usage(argv[0])
            exit(1)
    else:
        sx: int = 0
        sy: int = 0  # default

    if "-exy" in argv:
        pos = argv.index("-exy")
        param: str = argv[pos + 1]
        num_arr: list[int] = str_tuple_to_arr(param)
        if len(num_arr) == 2:
            argv.remove("-exy")
            argv.remove(param)
            ex: int = num_arr[0]
            ey: int = num_arr[1]
        else:
            print_usage(argv[0])
            exit(1)
    else:
        ex: int = -1
        ey: int = -1  # default

    if "-s" in argv:
        argv.remove("-s")
        show: bool = True
    else:
        show: bool = False  # default

    # TODO: check if they are paths or exist
    if "-p" in argv:
        input_file = input()
        input_file = input_file[1:]  # dirty fix
        if len(input_file) == 0:
            eprint("PIPING ERROR: did not received file name")
            exit(1)
    elif len(argv) == 2:
        input_file = argv[-1]
    else:
        print_usage(argv[0])
        exit(1)

    return (input_file, resolution, sx, sy, ex, ey, show)


def main(argv: list[str]):
    if len(argv) < 2:
        print_usage(argv[0])
        exit(1)

    (input_file, resolution, sx, sy, ex, ey, show) = parser(argv)

    if len(input_file) == 0:
        print("ERROR: no input file detected")
        print_usage(argv[0])
        exit(1)

    loaded_picture = []
    # open the files TODO: check if file exists
    with fits.open(input_file, mode='readonly') as base_file:
        if ex == -1 or ey == -1:  # TODO: make it optional
            loaded_picture = base_file[0].data[sx:, sy:]
        else:
            loaded_picture = base_file[0].data[sx:ex, sy:ey]

    hist, bins = np.histogram(loaded_picture, bins=resolution)

    if show:
        from matplotlib import pyplot as plt
        # plt.bar([str(i) for i in bins[:-1]],hist,color ='maroon')
        plt.plot(bins[1:], hist)
        plt.title("histogram")
        plt.show()
    else:
        print(f"histogram: {hist}")
        print(f"bins     : {bins}")


if __name__ == "__main__":
    main(sys.argv)
