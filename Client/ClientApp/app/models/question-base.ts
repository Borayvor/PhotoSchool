export class QuestionBase<T>{
  value: T;
  key: string;
  label: string;
  required: boolean;
  minLength: number;
  maxLength: number;
  order: number;
  controlType: string;

  errorMessage: string;

  constructor(options: {
    value?: T,
    key?: string,
    label?: string,
    required?: boolean,
    minLength?: number,
    maxLength?: number,
    order?: number,
    controlType?: string,
    errorMessage?: string
  } = {}) {
    this.value = options.value;
    this.key = options.key || '';
    this.label = options.label || '';
    this.required = !!options.required;
    this.minLength = options.minLength === undefined ? 0 : options.minLength;
    this.maxLength = options.maxLength === undefined ? Number.MAX_SAFE_INTEGER : options.maxLength;
    this.order = options.order === undefined ? 1 : options.order;
    this.controlType = options.controlType || '';
    this.errorMessage = options.errorMessage || '';
  }
}
