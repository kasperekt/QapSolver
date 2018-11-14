import matplotlib.pyplot as plt
import numpy as np
import re

from helpers import quality
from config import chart_size, chart_dpi


def similarity_percentage(solution, optimal):
    count = 0

    for s_el, o_el in zip(solution, optimal):
        if s_el == o_el:
            count += 1

    print(count, len(solution) / count if count != 0 else 0)
    return 0 if count == 0 else len(solution) / count


def get_optimum(file):
    with open(file, 'r') as fh:
        data = fh.read()

    return parse_optimal_solution(data)


def get_results(file):
    with open(file, 'r') as fh:
        data = fh.read()

    data = re.sub(' +', ' ', data).split('\n')

    optimal_result = int(data[0].split(' ')[1])
    optimal_perm = data[1].split(' ')

    return optimal_result, optimal_perm


def parse_optimal_solution(data):
    data = re.sub(' +', ' ', data).split('\n')

    optimal_result = int(data[0].split(' ')[2])
    optimal_perm = data[1].split(' ')[0:-1]

    return optimal_result, optimal_perm


algs = {
    'local-greedy-solver': {
        'label': 'greedy',
        'marker': 'o'
    },
    'local-steepest-solver': {
        'label': 'steepest',
        'marker': 'o'

    },
    'heuristic-solver': {
        'label': 'heuristic',
        'marker': 'o'
    },
    'random-solver': {
        'label': 'random',
        'marker': 'o'
    }
}

all_problems = ['chr18a', 'chr20a', 'esc32g',
                'lipa50a', 'tai12a', 'tai12b', 'tai15a', 'tai35b']
selected_problems = ['chr18a', 'chr20a']
size = 300
plt.figure(figsize=chart_size, dpi=chart_dpi)

x = np.arange(1, 301, 1)

for problem in selected_problems:
    optimal_score, optimal_permutation = get_optimum(
        '../QapData/' + problem + '.sln')

    alg_results = {}
    for alg in algs.keys():
        alg_results[alg] = {}
        alg_score, alg_permutation = get_results(
            '../QapSolver/results/' + problem + '_' + alg + '_' + str(size) + '.sln')
        alg_results[alg]['quality'] = quality(alg_score, optimal_score)
        alg_results[alg]['similarity'] = similarity_percentage(
            alg_permutation, optimal_permutation)

    for alg in algs.keys():
        plt.scatter(alg_results[alg]['quality'], alg_results[alg]['similarity'],
                    label=algs[alg]['label'] + "_" + problem, marker=algs[alg]['marker'],
                    alpha=0.75,
                    s=200)


plt.xlabel('jakość rozwiązania')
plt.ylabel('podobieństwo do optimum')
plt.legend(bbox_to_anchor=(0., 1.02, 1., .102), mode='expand', ncol=4, loc=3)
plt.savefig('quality_vs_similarity.png', dpi=chart_dpi)
