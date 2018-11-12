import matplotlib.pyplot as plt
import numpy as np
import re


def get_optimum(file):
    with open(file, 'r') as fh:
        data = fh.read()

    return parse_optimal_solution(data)[0]


def parse_optimal_solution(data):
    data = re.sub(' +', ' ', data).split('\n')

    optimal_result = int(data[0].split(' ')[2])
    optimal_perm = data[1].split(' ')[1:-1]

    return optimal_result, optimal_perm


def quality(solution, optimal):
    return 1 - ((solution - optimal) / solution)


def get_results(file):
    with open(file, 'r') as fh:
        data = fh.read()

    x = []
    y = []
    for i, line in enumerate(data.split('\n')):
        if i == 0 or i == (size+1):
            continue

        parsed = line.split(',')
        x.append(int(parsed[1]))
        y.append(int(parsed[2]))

    return x, y


algs = {
    'local-greedy-solver': {
        'label': 'greedy',
        'marker': 'v'
    },
    'local-steepest-solver': {
        'label': 'steepest',
        'marker': '^'
    }
}

all_problems = ['chr18a', 'chr20a', 'esc32g',
                'lipa50a', 'tai12a', 'tai12b', 'tai15a', 'tai35b']
selected_problems = ['chr18a', 'esc32g', 'lipa50a', 'tai12a', 'tai12b']
size = 200
plt.figure(figsize=(15, 10))

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

    for alg in algs.keys():
        plt.scatter(alg_results[alg]['initial'], alg_results[alg]['final'],
                    label=algs[alg]['label'] + "_" + problem, marker=algs[alg]['marker'],
                    alpha=0.75)

plt.xlabel('Jakość rozwiązania początkowego')
plt.ylabel('Jakość rozwiązania końcowego')
plt.legend()
plt.savefig('initial_vs_final.png')
