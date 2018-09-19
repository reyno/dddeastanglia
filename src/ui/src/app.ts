import { ProductsService, ProductModel } from './../services/products';
import { autoinject } from 'aurelia-framework';
import { CategoriesService, CategoryModel } from '../services/categories';
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
    private categoriesService: CategoriesService,
    private productsService: ProductsService
  ) { }

  async attached() {

    this.categories = await this.categoriesService.getAll();

  }

  async addCategory() {

    const value = await this.categoriesService.create(this.newCategory);

    this.categories.push(value);

    this.newCategory = { title: "" };

  }

  selectCategory(item) {
    this.selectedCategory = item;
    this.loadProducts();
  }

  async loadProducts() {
    this.products = await this.productsService.getForCategory(this.selectedCategory.id);
  }

  async addProduct() {

    const value = await this.productsService.create(
      this.selectedCategory.id,
      this.newProduct
    );

    this.products.push(value);

    this.newProduct = { title: "" };

  }


}
