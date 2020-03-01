import { FormMode } from '../base-enum/form-mode.enum';

export class BaseEntity {
    CreatedDate: Date;
    CreatedBy: string;
    ModifiedDate: Date;
    ModifiedBy: string;
    EditVersion: Date;
    State: FormMode;
}