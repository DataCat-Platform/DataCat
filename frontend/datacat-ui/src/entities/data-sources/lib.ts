import {
  ISearchFilters,
  MatchMode,
  SearchFieldType,
  SearchFilter,
  SearchFilters,
} from '../../shared/services/datacat-generated-client';
import { capitalizeFirstLetter } from '../../shared/utils/capitalizeFirstLetter';
import { DataSourcesFilter } from './data-sources-filter';

export const convertToApiFilters = (
  filter: DataSourcesFilter | undefined,
): SearchFilters => {
  let request = {
    filters: [],
    sort: undefined,
  } as ISearchFilters;

  if (filter?.name) {
    request.filters!.push({
      key: 'name',
      value: filter.name,
      matchMode: MatchMode.Equals,
      fieldType: SearchFieldType.String,
    } as SearchFilter);
  }

  if (filter?.typeId) {
    request.filters!.push({
      key: 'typeId',
      value: filter.typeId,
      matchMode: MatchMode.Equals,
      fieldType: SearchFieldType.Number,
    } as SearchFilter);
  }

  if (filter?.purpose) {
    request.filters!.push({
      key: 'purpose',
      value: capitalizeFirstLetter(filter.purpose),
      matchMode: MatchMode.Equals,
      fieldType: SearchFieldType.String,
    } as SearchFilter);
  }

  return request as SearchFilters;
};
