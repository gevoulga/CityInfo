```json
# Write your query or mutation here
query getProducts{
  products {
    id,
    name,
    ... on ProductType {
      description,
    }
  }
}

query productQuery($productId: ID!){
  product(id: $productId){
    id,
    name
    ... on ProductType{
      description
    }
  }
}

mutation createReview($review: reviewInput!){
    createReview(review:$review){
        id,
        title
    }
}

subscription reviewStream($productId: ID!){
  reviewAdded(productId:$productId){
    productId,
    title
  }
}
```


```json
{
    "review": {
        "title": "this is awesome!",
        "productId": 10
    },
  "productId": 10
}
```