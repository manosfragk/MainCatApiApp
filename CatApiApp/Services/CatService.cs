﻿using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CatApiApp.Services {
    /// <summary>
    /// Service for managing cat-related operations, including fetching and storing cat data from an external API.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CatService"/> class.
    /// </remarks>
    /// <param name="catApiClient">The client used to interact with the external Cat API.</param>
    /// <param name="context">The database context used to interact with the Cats and Tags entities.</param>
    public class CatService(ICatApiClient catApiClient, ICatRepository catRepository, ITagRepository tagRepository) : ICatService {
        private readonly ICatApiClient _catApiClient = catApiClient;
        private readonly ICatRepository _catRepository = catRepository;
        private readonly ITagRepository _tagRepository = tagRepository;

        /// <summary>
        /// Fetches 25 cat images from the external API and stores them in the database.
        /// </summary>
        public async Task FetchAndStoreCatsAsync() {
            var catImages = await _catApiClient.FetchCatImagesAsync();

            foreach (var cat in catImages) {
                // Check if the cat already exists in the database by CatId
                if (!_catRepository.CatExists(cat.Id)) {
                    var newCat = new CatEntity {
                        CatId = cat.Id,
                        Width = cat.Width,
                        Height = cat.Height,
                        Image = cat.Url,
                        Tags = await ExtractTagsFromBreed(cat.Breeds)
                    };
                    _catRepository.AddCat(newCat);
                }
            }
            await _catRepository.SaveAsync();
        }

        /// <summary>
        /// Extracts tags from cat breeds (temperament) and returns a list of associated tags.
        /// </summary>
        /// <param name="breeds">The list of breeds associated with the cat.</param>
        /// <returns>A list of <see cref="TagEntity"/> objects representing the cat's temperament.</returns>
        private async Task<List<TagEntity>> ExtractTagsFromBreed(List<CatBreed> breeds) {
            var tags = new List<TagEntity>();

            foreach (var breed in breeds) {
                var temperaments = breed.Temperament.Split(',');

                foreach (var temperament in temperaments) {
                    var trimmedTemperament = temperament.Trim();

                    // Check if the tag already exists in the database
                    var existingTag = await _tagRepository.GetTagByName(trimmedTemperament);

                    if (existingTag == null) {
                        // If the tag doesn't exist, create a new one
                        var newTag = new TagEntity { Name = trimmedTemperament, Created = DateTime.UtcNow };
                        await _tagRepository.AddTagAsync(newTag);
                        tags.Add(newTag);
                    }
                    else {
                        // Reuse the existing tag
                        tags.Add(existingTag);
                    }
                }
            }
            return tags;
        }
    }

}
