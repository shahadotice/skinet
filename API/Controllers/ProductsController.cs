
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
       
        
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,IGenericRepository<ProductBrand> productBrandRepo,IGenericRepository<ProductType> productTypeRepo,IMapper mapper)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
           
           

        }

        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<ProductToRerurnDto>>> GetProducts()
        {
            // var products = await _productRepo.ListAllAsync();
            var spec =new ProductsWithTypesAndBrandsSpecification();
             var products = await _productRepo.ListAsync(spec);
            //  return await _mapper.Map<Product,ProductToRerurnDto>(products);
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToRerurnDto>>(products));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToRerurnDto>> GetProduct(int id)
        {
              var spec =new ProductsWithTypesAndBrandsSpecification(id);
                //   var products = await _productRepo.GetbyIdAsync(id);
            var products = await _productRepo.GetEntityWithSpec(spec);
            if(products==null) return NotFound(new ApiResponse(404));

            return  _mapper.Map<Product,ProductToRerurnDto>(products);
            // return Ok(products);
        }


        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductBrandAsync()
        {
            var productBrands = await _productBrandRepo.ListAllAsync();
            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetProductTypesAsync()
        {
            var productTypes = await _productTypeRepo.ListAllAsync();
            return Ok(productTypes);
        }
    }
}