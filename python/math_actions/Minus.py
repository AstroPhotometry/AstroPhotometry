import numpy as np
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
        files_after_subtraction = []
        for file in self.files:
            outcome_array = np.subtract(file, self.pic)
            files_after_subtraction.append(outcome_array)

        return files_after_subtraction
