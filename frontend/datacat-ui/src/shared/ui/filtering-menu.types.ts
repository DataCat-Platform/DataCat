export type FilterOperand = DateFilterOperand;

export enum FilterOperandKind {
  DATE,
  SELECT,
};

export class DateFilterOperand {
  type: FilterOperandKind = FilterOperandKind.DATE;
}

export interface FilterOperation {
  variants: string[];
  selectedVariant: string;
  operand: FilterOperand;
}

export interface Filter {
  prefix: string;
  operation: FilterOperation;
}
