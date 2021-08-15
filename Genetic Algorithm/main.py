"""Main script to run sim"""
import pygame
from Dot import Dot
from Population import Population
from random import uniform
from math import dist
from copy import deepcopy
from Wall import Wall

WHITE = (255, 255, 255)
GREEN = (0, 255, 0)
RED = (255, 0, 0)
BLUE = (0, 0, 255)

SIM_COUNT = 50

min_step = 1002
max_dot = 0

pygame.init()
sx, sy = 600, 800
screen = pygame.display.set_mode([sx, sy])
sample = 1000

pop = Population(sample)

goal = Dot()
goal.goal = True
goal.x, goal.y = sx//2, sy//8

# -------- OBSTACLES --------
obstacles = [Wall(24, 200, 0, sy // 2),
             Wall(24, 200, sx - 200, 3 * sy // 4)]


def naturalSelection() -> None:
    """This selects the next generation!"""
    global pop, max_dot
    fitSum = sum([dot.fitness for dot in pop.dots])
    newDots = [Dot() for i in range(pop.size)]
    newDots[0].brain = deepcopy(pop.dots[max_dot].brain)
    newDots[0].brain.step = 0
    for i in range(1, pop.size):
        choice = uniform(0, fitSum)
        runSum = 0
        for j in range(pop.size):
            dot = pop.dots[j]
            runSum += dot.fitness
            if runSum > choice:
                newDots[i].brain = deepcopy(dot.brain)
                newDots[i].brain.step = 0
                break
    pop.dots = deepcopy(newDots)


def mutatePopulation() -> None:
    """This mutates a population"""
    global pop
    for i in range(1, pop.size):
        pop.dots[i].brain.mutate()


def setBest() -> None:
    """sets the best dot"""
    global pop, min_step, max_dot, reached_count
    reached_count = 0
    min_step = 1002

    maxDot = 0
    for i in range(pop.size):
        if pop.dots[i].fitness > pop.dots[maxDot].fitness:
            maxDot = i
        if pop.dots[i].reached and pop.dots[i].brain.step < min_step:
            min_step = pop.dots[i].brain.step
    pop.dots[maxDot].best = True


def dead(dot) -> bool:
    """Kills a dot if needed"""
    if dot.x >= sx:
        dot.x = sx
        return True
    if dot.y >= sy:
        dot.y = sy
        return True
    if dot.x <= 0:
        dot.x = 0
        return True
    if dot.y <= 0:
        dot.y = 0
        return True

    # obstacles
    for wall in obstacles:
        if wall.x < dot.x < wall.x + wall.width and wall.y < dot.y < wall.y + wall.height:
            return True


gen = 1
while gen < SIM_COUNT:
    # each generation
    pop.init_pos(sx, sy)  # reset dots in starting position
    while not pop.pop_dead():
        screen.fill(WHITE)
        goal.draw(screen)
        for wall in obstacles:
            wall.draw(screen)
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                run = False
                break

        # move dots
        for dot in pop.dots:
            dot.move()
            if dot.brain.step > min_step:
                dot.dead = True
            if dist((dot.x, dot.y), (goal.x, goal.y)) < 5:  # stop em if they reach goal
                dot.dead = True
                dot.reached = True
            if dead(dot):
                dot.dead = True  # check if they dead
        pop.draw_population(screen)

        pygame.display.flip()

    # calculate fitnesses
    pop.pop_fit(goal.x, goal.y)  # calc all fitnesses
    fit = [dot.fitness for dot in pop.dots]
    avg = sum(fit)/len(fit)

    print("GEN "+str(gen)+" AVG: "+str(avg*10**9))

    # set best dot for next gen
    setBest()

    # use fitnesses to generate new population
    naturalSelection()

    # mutate the children
    mutatePopulation()
    gen += 1

print(str(gen)+" generations")
pygame.quit()
