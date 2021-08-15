"""A dot's brain"""
import pygame
from typing import List, Tuple
import random
from math import pi
import random


class Brain:
    """The brain stores the moves and handles mutation of a Dot"""
    moves = List[float]
    step = 0
    size = 0

    def __init__(self, steps) -> None:
        self.moves = []
        self.size = steps
        for i in range(steps):
            self.moves.append(random.uniform(0, 2*pi))

    def mutate(self) -> None:
        """This method mutates the brain by a mutation_rate"""
        mutation_rate = 1
        for i in range(self.size):
            x = random.randint(0, 100)
            if x < mutation_rate:  # mutate this move
                self.moves[i] = random.uniform(0, 2 * pi)
