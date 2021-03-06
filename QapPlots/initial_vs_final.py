import matplotlib.pyplot as plt
import numpy as np
import re

from helpers import quality
from config import chart_size, chart_dpi


def get_optimum(file):
    with open(file, 'r') as fh:
        data = fh.read()

    return parse_optimal_solution(data)[0]


def parse_optimal_solution(data):
    data = re.sub(' +', ' ', data).split('\n')

    optimal_result = int(data[0].split(' ')[2])
    optimal_perm = data[1].split(' ')[1:-1]

    return optimal_result, optimal_perm


def get_results(file):
    with open(file, 'r') as fh:
        data = fh.read()

    x = []
    y = []
    for i, line in enumerate(data.split('\n')):
        if i == 0 or i == (size + 1):
            continue

        parsed = line.split(',')
        x.append(int(parsed[1]))
        y.append(int(parsed[2]))

    return x, y


algs = {
    'local-greedy-solver': {
        'label': 'LG',
        'marker': 'v',
        'color': 'g'
    },
    'local-steepest-solver': {
        'label': 'LS',
        'marker': '^',
        'color': 'y'
    },
    'simulated-annealing': {
        'label': 'SA',
        'marker': '*',
        'color': 'b'
    },
    'taboo': {
        'label': 'TS',
        'marker': 'p',
        'color': 'r'
    }
}

problem_markers = {
    'chr18a': '*',
    'esc32g': 'p',
    'lipa50a': '^',
    'tai12a': '.',
    'tai12b': 'v'
}

selected_problems = ['chr18a', 'esc32g', 'lipa50a', 'tai12a', 'tai12b']
size = 300
plt.figure(figsize=chart_size, dpi=chart_dpi)

for problem in selected_problems:
    optimal_score = get_optimum('../QapData/' + problem + '.sln')

    alg_results = {}
    for alg in algs.keys():
        alg_results[alg] = {}
        alg_data_x, alg_data_y = get_results(
            '../QapSolver/results/' + problem + '_' + alg + '_' + str(size) + '.csv')

        alg_results[alg]['initial'] = list(
            map(lambda x: quality(x, optimal_score), alg_data_x))
        alg_results[alg]['final'] = list(
            map(lambda y: quality(y, optimal_score), alg_data_y))

        alg_results[alg]['initial'] = alg_results[alg]['initial'][::3]
        alg_results[alg]['final'] = alg_results[alg]['final'][::3]

    for alg in algs.keys():
        plt.scatter(alg_results[alg]['initial'], alg_results[alg]['final'],
                    label=algs[alg]['label'] + "_" + problem,
                    marker=problem_markers[problem],
                    c=algs[alg]['color'],
                    alpha=0.75)

plt.xlabel('Jakość rozwiązania początkowego')
plt.ylabel('Jakość rozwiązania końcowego')
plt.legend()
plt.savefig('initial_vs_final.png', dpi=chart_dpi)
