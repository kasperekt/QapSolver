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

  best = []
  avg = []
  scores = []

  for i, line in enumerate(data.split('\n')):
    if i==0 or i == (size+1): continue
    parsed = line.split(',')

    score = int(parsed[1])
    if i==1:
        best.append(score)
        scores.append(score)
        avg.append(score)
        continue

    if score < best[i-2]:
        best.append(score)
    else:
        best.append(best[i-2])

    scores.append(score)
    avg.append(np.mean(scores))
  return best, avg


algs = {
    'local-greedy-solver': {
        'label': 'greedy',
        'linestyle': 'solid'

    },
    'local-steepest-solver': {
        'label': 'steepest',
        'linestyle': 'dashed'
    }
}

all_problems = ['chr18a', 'chr20a', 'esc32g', 'lipa50a', 'tai12a', 'tai12b', 'tai15a', 'tai35b']
selected_problems = ['tai12a', 'tai35b']
size = 300
plt.figure(figsize=(15,10))

x = np.arange(1, 301, 1)

for problem in selected_problems:
  optimal_score = get_optimum('../QapData/' + problem + '.sln')

  alg_results = {}
  for alg in algs.keys():
      alg_results[alg] = {}
      alg_data_best, alg_data_avg = get_results('../QapSolver/results/' + problem + '_' + alg + '_' + str(size) + '.csv')
      alg_results[alg]['best'] = list(map(lambda x: quality(x, optimal_score), alg_data_best))
      alg_results[alg]['avg'] = list(map(lambda y: quality(y, optimal_score), alg_data_avg))


  for alg in algs.keys():
      plt.plot(x, alg_results[alg]['best'],
                  label=algs[alg]['label'] + "_" + problem + "_" + 'best', alpha=0.8, linestyle=algs[alg]['linestyle'])

  for alg in algs.keys():
      plt.plot(x, alg_results[alg]['avg'],
                  label=algs[alg]['label'] + "_" + problem + "_" + 'avg', alpha=0.8, linestyle=algs[alg]['linestyle'])

plt.xlabel('liczba iteracji')
plt.ylabel('jakość rozwiązania')
plt.legend(bbox_to_anchor=(1.05, 1))
plt.savefig('multiple_restarts.png')

