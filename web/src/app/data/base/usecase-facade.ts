import { BaseEntity } from 'src/app/domain';
import { UseCase } from './use-case';

export abstract class BaseUseCaseFacade<T extends BaseEntity> {
  abstract get getUseCase(): UseCase<any, T[]>;
  abstract get createUseCase(): UseCase<T, string>;
  abstract get editUseCase(): UseCase<T, string>;
  abstract get deleteUseCase(): UseCase<string, string>;
}
