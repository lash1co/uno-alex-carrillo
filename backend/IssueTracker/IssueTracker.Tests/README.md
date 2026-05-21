# Pruebas Unitarias - IssueTracker

Este proyecto contiene pruebas unitarias e integración para la aplicación IssueTracker usando **xUnit**, **Moq** y **FluentAssertions**.

## Estructura del Proyecto

```
IssueTracker.Tests/
├── Domain/
│   └── Entities/
│       ├── IssueTests.cs          # Pruebas para la entidad Issue
│       ├── AttachmentTests.cs     # Pruebas para la entidad Attachment
│       └── UserTests.cs           # Pruebas para la entidad User
├── Application/
│   ├── Services/
│   │   └── IssueServiceTests.cs   # Pruebas para el servicio de Issues
│   └── Validators/
│       └── CreateIssueDtoValidatorTests.cs  # Pruebas para validadores
└── GlobalUsings.cs               # Imports globales
```

## Dependencias

- **xunit** (2.8.1) - Framework de pruebas
- **xunit.runner.visualstudio** (2.5.8) - Integración con Visual Studio
- **Microsoft.NET.Test.Sdk** (17.11.1) - SDK de pruebas
- **Moq** (4.20.70) - Biblioteca de mocks
- **FluentAssertions** (6.12.1) - Assertions fluidas

## Ejecutar las Pruebas

### Desde Visual Studio
1. Abre el **Test Explorer** (Test > Test Explorer o Ctrl+E, T)
2. Haz clic en **Ejecutar todo** o selecciona pruebas específicas
3. Observa los resultados en la ventana de Test Explorer

### Desde la Terminal (PowerShell)

```powershell
# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas de un proyecto específico
dotnet test --filter "FullyQualifiedName~IssueTracker.Tests.Domain"

# Ejecutar una prueba específica
dotnet test --filter "FullyQualifiedName=IssueTracker.Tests.Domain.Entities.IssueTests.Issue_ShouldHaveDefaultValues_WhenCreated"

# Ejecutar con verbosidad
dotnet test -v detailed

# Generar reporte de cobertura
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## Patrones de Prueba

### 1. Pruebas de Entidades (AAA Pattern)
```csharp
[Fact]
public void Issue_ShouldHaveDefaultValues_WhenCreated()
{
	// Arrange - Preparación
	var issue = new Issue();

	// Act - Acción
	// (ya realizada en Arrange)

	// Assert - Verificación
	issue.Status.Should().Be(IssueStatus.Open);
}
```

### 2. Pruebas Paramétricas (Theory)
```csharp
[Theory]
[InlineData(IssuePriority.Low)]
[InlineData(IssuePriority.High)]
public void Issue_ShouldSupportAllPriorityValues(IssuePriority priority)
{
	var issue = new Issue { Priority = priority };
	issue.Priority.Should().Be(priority);
}
```

### 3. Pruebas de Servicios con Mocks
```csharp
[Fact]
public async Task CreateIssueAsync_WithValidData_ShouldCreateIssue()
{
	// Arrange
	var mockRepository = new Mock<IRepository<Issue>>();
	mockRepository.Setup(r => r.AddAsync(It.IsAny<Issue>()))
		.ReturnsAsync(expectedIssue);

	// Act
	var result = await issueService.CreateIssueAsync(dto);

	// Assert
	result.Should().NotBeNull();
	mockRepository.Verify(r => r.AddAsync(It.IsAny<Issue>()), Times.Once);
}
```

### 4. Pruebas de Validadores
```csharp
[Fact]
public async Task Validate_WithValidData_ShouldPass()
{
	var dto = new CreateIssueDto { Title = "Valid", Description = "Valid" };
	var result = await _validator.ValidateAsync(dto);
	result.IsValid.Should().BeTrue();
}
```

## Convenciones de Nombres

- **Tests**: `[ClassName]Tests.cs`
- **Métodos de prueba**: `[MethodName]_[Condition]_[ExpectedResult]`
  - Ejemplo: `CreateIssueAsync_WithValidData_ShouldCreateIssue`

## Assertion Fluidas con FluentAssertions

```csharp
// Comparaciones básicas
result.Should().Be(expected);
result.Should().NotBeNull();
result.Should().BeEmpty();

// Colecciones
list.Should().HaveCount(3);
list.Should().Contain(item);
list.First().Should().Be(firstItem);

// Strings
name.Should().StartWith("John");
email.Should().Contain("@");

// Excepciones
await Assert.ThrowsAsync<NotFoundException>(() => method());
```

## Mocking con Moq

```csharp
// Crear mock
var mockRepository = new Mock<IRepository<Issue>>();

// Configurar comportamiento
mockRepository
	.Setup(r => r.GetByIdAsync(id))
	.ReturnsAsync(issue);

// Verificar que fue llamado
mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
```

## Añadir Nuevas Pruebas

1. Crea un archivo `[Class]Tests.cs` en la carpeta correspondiente
2. Hereda el patrón AAA (Arrange-Act-Assert)
3. Usa nombres descriptivos para los métodos de prueba
4. Ejecuta `dotnet test` para validar

## Cobertura de Código

Para generar un reporte de cobertura, necesitas instalar **ReportGenerator**:

```powershell
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generar reporte de cobertura
dotnet test /p:CollectCoverage=true
reportgenerator "-reports:**/coverage.opencover.xml" "-targetdir:coverage" -reporttypes:HtmlInline
```

## Recursos

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Microsoft Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/)
