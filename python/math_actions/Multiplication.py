import numpy as np
from math_actions.BaseMath import BaseMath


class Multiplication(BaseMath):
    def __init__(self, files, pic):
        self.pic = pic
        self.files = files

    def compute(self):
        """
        Method to compute multiplication
        :return:
        """
        files_after_multiply = []
        for file in self.files:
            outcome_array = np.multiply(file, self.pic)
            files_after_multiply.append(outcome_array)

        return files_after_multiply
