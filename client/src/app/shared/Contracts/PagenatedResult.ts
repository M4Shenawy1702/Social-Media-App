export interface PagenatedResult<TType> {
  pageIndex: number;
  pageCount: number;
  count: number;
  data: TType[];
}
