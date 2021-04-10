export type FunctionWithParametersType<P extends unknown[], R = void> = (
  ...args: P
) => R;
