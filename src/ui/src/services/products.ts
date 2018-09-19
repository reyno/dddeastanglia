import { CategoryModel } from './categories';
import { autoinject } from 'aurelia-framework';
import { HttpClient, json } from 'aurelia-fetch-client';

export class ProductModel {
  id?: number;
  title: string;
  description?: string;
}

@autoinject
export class ProductsService {

  constructor(
    private httpClient: HttpClient
  ) { }

  async getForCategory(category: CategoryModel | number): Promise<ProductModel[]> {

    const categoryId = typeof category === "number" ? category : category.id;

    const response = await this
      .httpClient
      .fetch(`api/categories/${categoryId}/products`);

    return await response.json();

  }


  async create(categoryId: number, value: ProductModel): Promise<ProductModel> {

    const response = await this
      .httpClient
      .fetch(`api/categories/${categoryId}/products`, {
        method: "POST",
        body: json(value)
      });

    return await response.json();

  }

}
