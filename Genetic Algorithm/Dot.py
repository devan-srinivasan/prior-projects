""" Dot class"""
from typing import Tuple, List
from Brain import Brain
import pygame
from math import sin, cos, dist

BLACK = (0, 0, 0)
BEST = (255, 255, 0)
GREEN = (0, 255, 0)


class Dot:
    """This is the Dot class. It has a brain. It can be drawn to the pygame window as well."""
    x: int
    y: int
    color: Tuple[int, int, int]
    dead: bool
    brain: Brain
    vel: int
    rad: int
    fitness: float
    reached: bool
    best: bool
    goal: bool

    def __init__(self) -> None:
        self.color = BLACK
        self.dead = False
        self.brain = Brain(1000)
        self.vel = 10
        self.rad = 2
        self.fitness = 0.0
        self.reached = False
        self.best = False
        self.goal = False

    def draw(self, screen) -> None:
        """This method draws the dot to screen"""
        if self.best:
            self.color = BEST
            self.rad = 2
        elif self.goal:
            self.color = GREEN
            self.rad = 9
        else:
            self.color = BLACK
            self.rad = 2
        pygame.draw.circle(screen, self.color, (self.x, self.y), self.rad)

    def move(self) -> None:
        """This method moves the dot by Brain's steps unless the dot is dead"""
        if self.brain.step < self.brain.size:
            if not self.dead:
                theta = self.brain.moves[self.brain.step]
                self.x += self.vel * cos(theta)
                self.y += self.vel * sin(theta)
                self.brain.step += 1
        else:
            self.dead = True

    def calc_fit(self, gx, gy) -> None:
        """This method calculates the Dot's fitness relative to the goal it needs to reach.
        The Dot is not aware of the goal's coordinates. This fitness score is used for the next
        population"""
        if not self.reached:
            pts = dist((self.x, self.y), (gx, gy))
            self.fitness = 1.0/(pts*pts)
        else:
            self.fitness = 1/16 + 10000/(self.brain.step*self.brain.step)
