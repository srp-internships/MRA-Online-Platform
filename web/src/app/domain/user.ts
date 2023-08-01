import { BaseEntity } from './base-entity';
import { List } from './list';

export class User extends BaseEntity implements List {
  get name(): string {
    return this.firstName + ' ' + this.lastName;
  }
  email!: string;
  password!: string;
  firstName!: string;
  lastName!: string;
  phoneNumber!: string;
  dateOfBirth!: Date;
  country!: string;
  region!: string;
  city!: string;
  address!: string;
}
