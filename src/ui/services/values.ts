import { autoinject } from 'aurelia-framework';
import { HttpClient, json } from 'aurelia-fetch-client';

export class ValueModel {
  id: number;
  value: string;
}

@autoinject
export class ValueService {

  constructor(
    private httpClient: HttpClient
  ) { }

  async getAll(): Promise<ValueModel[]> {

    const response = await this
      .httpClient
      .fetch('api/values');

    return await response.json();

  }

  async get(id?: number): Promise<ValueModel> {

    const response = await this
      .httpClient
      .fetch(`api/values/${id}`);

    return await response.json();

  }

  async create(value: string): Promise<ValueModel> {

    const response = await this
      .httpClient
      .fetch(`api/values`, {
        method: "POST",
        body: json({ value })
      });

    return await response.json();

  }

  async update(id: number, value: string): Promise<void> {

    const response = await this
      .httpClient
      .fetch(`api/values/${id}`, {
        method: "PUT",
        body: json({ id, value })
      });

  }

  async delete(id: number): Promise<void> {

    const response = await this
      .httpClient
      .fetch(`api/values/${id}`, {
        method: "DELETE"
      });

  }

}
