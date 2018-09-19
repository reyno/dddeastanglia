import { autoinject } from 'aurelia-framework';
import { mediator } from 'services/mediator';

import './app.less';

@autoinject
export class App {

  message = 'Hello World!';
  categories: CategoryModel[];
  newCategory: CategoryModel = { title: "" };
  selectedCategory: CategoryModel;
  products: ProductModel[];
  newProduct: ProductModel = { title: "" };

  constructor(
  ) { }

  async attached() {

    this.categories = await mediator.send("categories.getAll");

  }

  async addCategory() {

    const value = await mediator.send("categories.create", this.newCategory);

    this.categories.push(value);

    this.newCategory = { title: "" };

  }

  selectCategory(item) {
    this.selectedCategory = item;
    this.loadProducts();
  }

  async loadProducts() {
    this.products = await mediator.send("categories.products.getAll", { categoryId: this.selectedCategory.id });
  }

  async addProduct() {

    const categoryId = this.selectedCategory.id;
    const data = {
      ...this.newProduct,
      ...{ categoryId }
    };

    const value = await mediator.send("categories.products.create", data);

    this.products.push(value);

    this.newProduct = { title: "" };

  }


}
