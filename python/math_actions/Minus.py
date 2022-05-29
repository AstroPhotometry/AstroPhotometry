import numpy as np
from math_actions.BaseMath import BaseMath


class Minus(BaseMath):
    def __init__(self, files, pic=None):
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

    def subtract_two_images(self):
        return np.subtract(self.files[0], self.files[1])
