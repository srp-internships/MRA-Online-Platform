export class PathResolver {
  constructor(private readonly path: string) {}

  resolve(data: Record<string, string>): string {
    let result = this.path;
    const paths = result.split('/');
    const map = new Map(Object.entries(data));
    for (const path of paths) {
      if (path.startsWith(':')) {
        const key = path.substring(1);
        const value = map.get(key);
        if (value) {
          result = result.replace(`:${key}`, value);
        }
      }
    }
    return result;
  }
}
