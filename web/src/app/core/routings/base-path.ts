export interface BasePath {
  readonly base: string;
  getPath: (link?: any) => string;
}

export abstract class ModulePath implements BasePath {
  abstract readonly base: string;
  constructor(private parentModule?: BasePath) {}
  getPath(link?: any): string {
    const parentPath = this.parentModule?.getPath();
    return [parentPath, this.base, link].filter(x => x).join('/');
  }
}
