import numpy as np
from math_actions.BaseMath import BaseMath


class Average(BaseMath):

    def __init__(self, files):
        self.files = files

    def compute(self):
        """
        Method to compute average
        :return:
        """
        return np.mean(self.files, axis=0)
