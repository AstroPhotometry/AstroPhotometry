import numpy
from astropy.io import fits


class Minus:
    def __init__(self, pic, files):
        self.pic = pic
        self.files = files

    def compute(self):
        """
        Method to compute minus
        :return:
        """
        for file in self.files:
            with fits.open(file, mode='readonly') as next_file:
                tmp = out_picture - next_file[
                                        0].data[:, :]  # Has tmp for mixing ints and floats array math
                out_picture = tmp
