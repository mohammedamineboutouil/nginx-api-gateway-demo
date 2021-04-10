/// <reference lib="webworker" />

addEventListener('message', ({ data }) => {
  let fib: number = +data;

  let prev = 0;
  const results = [prev];

  if (fib <= 0) {
    postMessage(results);
    return;
  }

  let current = 1;

  results.push(current);

  if (fib <= 0) {
    postMessage(results);
    return;
  }

  let next = 0;

  for (let i = 0; i < fib; i++) {
    next = prev + current;
    prev = current;
    current = next;
    results.push(current);
  }

  postMessage(JSON.stringify(results));
});
