def quality(solution, optimal):
    '''Optimal is always less or equal.'''
    return optimal / solution


def effectiveness(quality, time):
    if time == 0:
        return 0

    return quality / time
