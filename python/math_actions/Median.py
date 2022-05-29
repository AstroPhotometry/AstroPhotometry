import numpy as np
from math_actions.BaseMath import BaseMath


class Median(BaseMath):

    def __init__(self, files):
        self.files = files

    def compute(self):
        """
        Method to compute average
        :return:
        """
        out = np.array(self.files)
        return np.median(out, axis=0)
