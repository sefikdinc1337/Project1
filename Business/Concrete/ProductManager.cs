using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilites.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ValidationException = FluentValidation.ValidationException;
using System.Linq;
using Core.Utilites.Business;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService  _categoryService;

        public ProductManager(IProductDal productDal,ICategoryService categoryService )
        {
            _productDal = productDal;
            _categoryService = categoryService;
            
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
           IResult result =  BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryID), CheckIfProductNameIsExists(product.ProductName), CheckIfNumberOfCategoryIsHigher());

            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
                    return new SuccessResult(Messages.ProductAdded);
 
            
        }

        public IDataResult<List<Product>> GetAll()
        {
            //İş Kodları
            if (DateTime.Now.Hour==11)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.GetAllSuccess);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            if (DateTime.Now.Hour == 11)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new  SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryID == id),Messages.GetAllSuccess);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new  SuccessDataResult<Product>(_productDal.Get(p => p.ProductID == productId),Messages.GetAllSuccess);
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new  SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max),Messages.GetAllSuccess);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new  SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(),"Verilerin detayları getirildi.");

        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            
            throw new NotImplementedException();
        }
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryID)
        {
            var result = _productDal.GetAll(p => p.CategoryID == categoryID).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameIsExists(string productName)
        {
            var result = _productDal.Get(p => p.ProductName == productName);
            if (result.ProductName == productName)
            {
                return new ErrorResult(Messages.ProductNameAlreadyUsed);
            }
            return new SuccessResult();
        }
        private IResult CheckIfNumberOfCategoryIsHigher()
        {
            var result = _categoryService.GetAll(); // Explicitly access the Data property
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryCountIsHigher);
            }
            return new SuccessResult();
        }

    }
}
