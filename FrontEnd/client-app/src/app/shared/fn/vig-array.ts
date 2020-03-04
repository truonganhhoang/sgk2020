import * as _ from "lodash";

export class CRMArray {

  public static Every(collection: any, predicate: any) {
    return _.every(collection, predicate);
  }

  public static Some(collection: any, predicate: any) {
    return _.some(collection, predicate);
  }

  public static Head(collection: any) {
    return _.head(collection);
  }

  public static UniqBy(array, iteratee) {
    return _.uniqBy(array, iteratee);
  }

  public static PullAllBy(array, values, iteratee) {
    return _.pullAllBy(array, values, iteratee);
  }

  public static SortBy(colection, prop, type = 'asc') {
    const compare = (a, b) => {
      if (a[prop] < b[prop]) {
        if (type === 'desc') {
          return 1;
        }
        return -1;
      }
      if (a[prop] > b[prop]) {
        if (type === 'desc') {
          return -1;
        }
        return 1;
      }
      return 0;
    }
    return colection.sort(compare);
  }

  public static Take(array, startAt = 1) {
    return _.take(array, startAt);
  }
}
