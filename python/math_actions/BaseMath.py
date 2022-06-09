from abc import ABC, abstractmethod


class BaseMath(ABC):

    @abstractmethod
    def compute(self):
        pass
