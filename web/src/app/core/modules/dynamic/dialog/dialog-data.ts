import { UseCase } from 'src/app/data/base/use-case';
import { BaseEntity } from 'src/app/domain';

export class DynamicDialogData<T extends BaseEntity = BaseEntity> {
  useCase?: UseCase<T, any>;
  primaryButtonText: string;
  title: string;

  constructor(
    public entity: T,
    options: {
      dialogTitle: string;
      primaryButtonText: string;
      useCase?: UseCase<T, any>;
    }
  ) {
    this.title = options.dialogTitle || '';
    this.primaryButtonText = options.primaryButtonText;
    this.useCase = options?.useCase;
  }
}
