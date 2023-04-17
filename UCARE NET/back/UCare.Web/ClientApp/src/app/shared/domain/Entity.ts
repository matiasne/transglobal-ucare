export abstract class Entity {
  [name: string]: any
  constructor(data: any = undefined) {
    if (data !== undefined && data !== null) {
      Object.getOwnPropertyNames(this).forEach((key) => {
        if (data[key] !== undefined) {
          this[key] = data[key];
        }
      });
    }
  }
}
