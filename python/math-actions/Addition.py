import numpy as np
from astropy.io import fits


class Addition:
    def __init__(self, pic, files):
        self.pic = pic
        self.files = files

    def compute(self):
        """
        Method to compute addition
        :return:
        """
        for file in self.files:
            with fits.open(file, mode='readonly') as next_file:
                self.pic.append(next_file[0].data[:, :])
        return np.sum(self.pic, axis=0)
