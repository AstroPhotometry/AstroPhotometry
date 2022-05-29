import numpy as np
from math_actions.BaseMath import BaseMath


class Addition(BaseMath):

    def __init__(self, files):
        self.files = files

    def compute(self):
        """
        Method to compute addition
        :return:
        """
        return np.sum(self.files, axis=0)
