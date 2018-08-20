import { autoinject } from 'aurelia-framework';
import { ValueService, ValueModel } from '../services/values';

@autoinject
export class App {

  message = 'Hello World!';
  values: ValueModel[];
  newValue: string;

  constructor(
    private valueService: ValueService
  ) { }

  async attached() {

    this.values = await this.valueService.getAll();

  }

  async addNewValue() {

    const value = await this.valueService.create(this.newValue);

    this.values.push(value);

  }

  async delete(item) {

    event.stopImmediatePropagation();

    await this.valueService.delete(item.id);

    this.values.splice(
      this.values.findIndex(x => x.id === item.id),
      1
    );

  }
}
