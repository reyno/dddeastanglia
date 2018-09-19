interface IMediator {

  send(name: "categories.getAll"): Promise<CategoryModel[]>;
  send(name: "categories.create", request: CategoryModel): Promise<CategoryModel>;
  send(name: "categories.products.getAll", request: { categoryId: number }): Promise<CategoryModel[]>;
  send(name: "categories.products.create", request: CategoryProductCreateRequest): Promise<CategoryModel>;

}

interface CategoryProductCreateRequest extends ProductModel {
  categoryId: number;
}

interface CategoryModel {
  id?: number;
  title: string;
  description?: string;
}

interface ProductModel {
  id?: number;
  title: string;
  description?: string;
}
