export interface FilterOperand {}

export interface FilterOperation {
  variants: string[];
  selectedVariant: string;
  operand: FilterOperand;
}

export interface Filter {
  prefix: string;
  operation: FilterOperation;
}
