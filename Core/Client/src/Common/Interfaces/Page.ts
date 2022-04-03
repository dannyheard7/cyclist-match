export interface Page<T> {
    hasPreviousPage: boolean;
    hasNextPage: boolean;
    pageNumber: number;
    totalCount: number;
    items: Array<T>;
}
