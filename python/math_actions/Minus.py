import numpy as np
from math_actions.BaseMath import BaseMath


class Minus(BaseMath):
    def __init__(self, files, pic):
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
