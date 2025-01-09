function generateSequence(start: number, end: number, step: number, filterFn: (n: number) => boolean = () => true): number[] {
  let sequence: number[] = [];

  for (let i: number = start; i <= end; i += step) {
    if (!filterFn(i)) continue;
    sequence.push(i);
  }

  return sequence;
}

export function generateSequenceWithStep(start: number, end: number, step: number): number[] {
  return generateSequence(start, end, step);
}

export function generateSequenceWithReverseStep(start: number, end: number, step: number, reverseStep: number): number[] {
  const sequence: number[] = generateSequence(start, end, step);
  let result: number[] = [];

  for (let i: number = 0; i < sequence.length;) {
    let segment: number[] = sequence.slice(i, i + reverseStep);
    result.push(...segment.reverse());
    i += reverseStep;
  }

  return result;
}

export function generateEvenSequence(start: number, end: number): number[] {
  return generateSequence(start, end, 1, (n: number): boolean => n % 2 === 0);
}

export function generateOddSequence(start: number, end: number): number[] {
  return generateSequence(start, end, 1, (n: number): boolean => n % 2 !== 0);
}
