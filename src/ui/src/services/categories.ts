import { autoinject } from 'aurelia-framework';
import { HttpClient, json } from 'aurelia-fetch-client';

export class CategoryModel {
  id?: number;
  title: string;
  description?: string;
}

@autoinject
export class CategoriesService {

  constructor(
    private httpClient: HttpClient
  ) { }

  async getAll(): Promise<CategoryModel[]> {

    const response = await this
      .httpClient
      .fetch('api/categories');

    return await response.json();

  }

  async create(value: CategoryModel): Promise<CategoryModel> {

    const response = await this
      .httpClient
      .fetch(`api/categories`, {
        method: "POST",
        body: json(value)
      });

    return await response.json();

  }

}
