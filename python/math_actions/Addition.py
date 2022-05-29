import numpy as np
from math_actions.BaseMath import BaseMath


class Addition(BaseMath):

    def __init__(self, image1, image2):
        self.image1 = image1
        self.image2 = image2

    def compute(self):
        """
        Method to compute addition
        :return:
        """
        out = np.array([self.image1, self.image2])
        return np.sum(out, axis=0)
