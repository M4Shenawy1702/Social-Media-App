export interface PagenatedResult<TType> {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: TType[];
}
