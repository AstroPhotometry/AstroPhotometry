import numpy as np
from astropy.io import fits
from math_actions import BaseMath


class Average(BaseMath):

    def __init__(self, pic, files):
        self.pic = pic
        self.files = files

    def compute(self):
        """
        Method to compute average
        :return:
        """
        for file in self.files:
            with fits.open(file, mode='readonly') as next_file:
                self.pic.append(next_file[0].data[:, :])
        return np.mean(self.pic, axis=0)
