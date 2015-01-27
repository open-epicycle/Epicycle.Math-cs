# Epicycle.Math-cs 0.1.6.0 [IN DEVELOPMENT]
Epicycle .NET math library. Includes: 2D/3D geometry, linear algebra, differential geometry, stochastic processes, Kalman filter.

***Note***: *This library is in it's 0.X version, that means that it's still in active development and backward compatibility is not guaranteed!*

## Links
* NuGet package: https://www.nuget.org/packages/Epicycle.Math-cs
* Git repository: https://github.com/open-epicycle/Epicycle.Math-cs
* All Epicycle Git repositories: https://github.com/open-epicycle

## Main features
* 2D and 3D geometric primitives and operations
* 2D and 3D multi-polygons that support boolean operations and offsets
* Differential geometry (manifolds)
* Linear algebra (vectors and matrices)
* Calculus
* Stochastic processes
* Kalman filter
* Math related unit test utilities
* Supported frameworks: **.NET 4.5**, **.NET 4.0**, **.NET 3.5**

## Namespaces
* **Epicycle.Math**:
  * Combinatorics utilities (Factorials and Binomials)
  * Solvable **CubicEquation**
* **Epicycle.Math.Calculus**:
  * Derivatives and interpolations
* **Epicycle.Math.Geometry**:
  * Various 2D and 3D geometric primitives. Including:
    * Vectors
    * Matrices
    * Intervals, lines and rays
    * Planes
    * Rotations and roto-translations
  * Various useful utilities. Including:
    * Intersections
    * Distances
    * Cropping
    * Projections
    * Accurate implementation of the Gram–Schmidt process
    * Rotating and rototranslating
* **Epicycle.Math.Geometry.Polytopes**:
  * 2D/3D open/closed polylines
  * 2D/3D polygons and multi-polygons
  * 3D polysurfaces and polyhedrons
  * Vertical prisms
* **Epicycle.Math.Geometry.Differential**:
  * Differential geometry library
  * Defines **IManifold**
  * **Rotation2Manifold** and **Rotation3Manifold**
* **Epicycle.Math.LinearAlgebra**:
  * Vectors and matrices of variable dimension
* **Epicycle.Math.Probability**:
  * Various types of stochastic processes
  * Kalman filter
* **Epicycle.Math.TestUtils**: (separate assembly)
  * Utilities for unit tests

## License
Apache License 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
Copyright 2015 Epicycle (http://epicycle.org)

## Release Notes
### Version 0.1 

* **Version 0.1.6** [IN DEVELOPMENT]

* **Version 0.1.5** [2015-01-27]
  * BUG FIX: Epicycle.Math.Geometry.Quaternion.Dot is wrong

* **Version 0.1.4** [2015-01-26]
  * Adding Epicycle.Math.Calculus.Derivatives
  * Adding Epicycle.Math.Calculus.Interpolation

* **Version 0.1.3** [2015-01-13]
  * Moving Epicycle.Math.Windows to Epicycle.Graphics

* **Version 0.1.2** [2015-01-12]
  * Adding support for **.NET 3.5**
  * Upgrading to Epicycle.Commons-cs.0.1.5.0
  * Upgrading to MathNet.Numerics.3.4.0

* **Version 0.1.1** [2015-01-08]
  * Adding support for **.NET 4.0**
  * Fixing namespaces

* **Version 0.1.0** [2015-01-08]
  * Initial release
