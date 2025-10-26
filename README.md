# LINQ Practice Project (C#)

A comprehensive C# console application demonstrating **LINQ (Language Integrated Query)** operations on in-memory collections.  
This project is designed for learning and practicing different LINQ concepts using `List<employee>` and `List<department>` objects.

---

## 📘 Overview

This project showcases a variety of LINQ operations — from basic filtering and ordering to joins, projections, grouping, and extension methods.  
It helps learners understand how LINQ works in real-world C# data scenarios.

---

## 🧩 Features

### 🔹 Basic LINQ Queries
- Select specific fields from a list
- Filter employees by gender, salary, or department
- Sort data in ascending and descending order

### 🔹 Advanced LINQ
- **Projection:** Transform results into anonymous types or custom objects  
- **Grouping:** Group employees by department or gender  
- **Join Operations:** Inner join, left join, and cross join between employees and departments  
- **Aggregation:** Calculate total, average, min, and max salaries

### 🔹 Extension Methods
- Demonstrates how to create custom LINQ-style extension methods like:
  ```csharp
  employees.gethighsalaries()
